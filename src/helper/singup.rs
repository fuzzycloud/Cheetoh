#[cfg(feature = "ssr")]
#[derive(Debug, serde::Deserialize, serde::Serialize)]
pub struct AuthData {
    pub email: String,
    pub password: String,
}

#[actix_web::post("/signup")]
#[cfg(feature = "ssr")]
pub async fn signup(
    form_data: actix_web::web::Form<AuthData>,
    read_pool: actix_web::web::Data<sqlx::SqlitePool>,
) -> Result<impl actix_web::Responder, actix_web::Error> {
    use sqlx::types::Uuid;

    let email = form_data.email.clone();
    let password = form_data.password.clone();

    let mut tx = read_pool.begin().await.map_err(|e| {
        actix_web::error::ErrorInternalServerError(format!("Failed to begin transaction: {}", e))
    })?;

    sqlx::query(
        r#"
        INSERT INTO auth (id, email, password) VALUES ($1, $2, $3) 
        "#,
    )
    .bind(Uuid::new_v4().to_string())
    .bind(email.clone())
    .bind(password.clone())
    .execute(&mut *tx)
    .await
    .map_err(|e| {
        actix_web::error::ErrorInternalServerError(format!("Failed to execute SQL query: {}", e))
    })?;

    tx.commit().await.map_err(|e| {
        actix_web::error::ErrorInternalServerError(format!("Failed to commit transaction: {}", e))
    })?;

    Ok(actix_web::HttpResponse::Found()
        .append_header((actix_web::http::header::LOCATION, "/home_page"))
        .finish())
}
