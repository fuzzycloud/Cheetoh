#[cfg(feature = "ssr")]
pub async fn get_blogs(state: crate::dto::app_state::AppState) -> Result<Vec<super::blog::BlogData>, actix_web::Error> {
 
    use sqlx::Row;

    let query_result = sqlx::query("SELECT title, content FROM blog")
        .fetch_all(&state.read_pool)
        .await
        .map_err(|e| {
            actix_web::error::ErrorInternalServerError(format!(
                "Failed to fetch blogs: {}",
                e
            ))
        })?;


    let blogs = query_result
        .into_iter()
        .map(|row| {
            super::blog::BlogData {
                title: row.get("title"),
                content: row.get("content"),
            }
        })
        .collect();

    Ok(blogs)
}
