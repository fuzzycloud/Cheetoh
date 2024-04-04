#[cfg(feature = "ssr")]
#[derive(Debug, serde::Deserialize, serde::Serialize, sqlx::FromRow)]
pub struct BlogData {
    pub title: String,
    pub content: String,
}

#[actix_web::post("/blog")]
#[cfg(feature = "ssr")]
pub async fn blog(
    form_data: actix_web::web::Form<BlogData>,
    read_pool: actix_web::web::Data<sqlx::SqlitePool>,
) -> Result<impl actix_web::Responder, actix_web::Error> {
    use sqlx::types::Uuid;

    let title = form_data.title.clone();
    let content = form_data.content.clone();

    let mut tx = read_pool.begin().await.map_err(|e| {
        actix_web::error::ErrorInternalServerError(format!("Failed to begin transaction: {}", e))
    })?;

    sqlx::query(
        r#"
        INSERT INTO blog (id, title, content) VALUES ($1, $2, $3) 
        "#,
    )
    .bind(Uuid::new_v4().to_string())
    .bind(title.clone())
    .bind(content.clone())
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
