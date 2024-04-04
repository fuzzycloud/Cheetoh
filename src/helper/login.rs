#[actix_web::post("/")]
#[cfg(feature = "ssr")]
pub async fn login(
    form_data: actix_web::web::Form<crate::helper::singup::AuthData>,
    read_pool: actix_web::web::Data<sqlx::SqlitePool>,
) -> Result<impl actix_web::Responder, actix_web::Error> {
    use log::error;
    use serde_json::json;
    use sqlx::Row;

    let email = form_data.email.clone();
    let password = form_data.password.clone();

    let query_result = sqlx::query("SELECT id, email, password FROM auth WHERE email = $1")
        .bind(&email)
        .fetch_optional(&**read_pool)
        .await
        .map_err(|e| {
            actix_web::error::ErrorInternalServerError(format!(
                "Failed to begin transaction: {}",
                e
            ))
        })?;

    if let Some(row) = query_result {
        // let id: &str = row.get("id");
        // let email: &str = row.get("email");
        let password_data: &str = row.get("password");

        if password == password_data {
            Ok(actix_web::HttpResponse::Found()
                .append_header((actix_web::http::header::LOCATION, "/home_page"))
                .finish())
        } else {
            error!("Password or email not Valid.");

            Err(actix_web::error::ErrorNotFound(json!({
                "message": "Password Or Email Not Valid",
                "code": 404,
            })))
        }
    } else {
        error!("User data not found in the database.");
        Err(actix_web::error::ErrorNotFound(json!({
            "message": "User not found",
            "code": 404,
        })))
    }
}
