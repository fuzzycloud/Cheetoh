use actix_web::web;
use serde::{Deserialize, Serialize};
use sqlx::Sqlite;

#[derive(Serialize, Deserialize, Debug, sqlx::FromRow)]
pub struct GetBlogs {
    pub id: String,
    pub title: String,
    pub content: String,
    pub created_at: String,
}

pub async fn get_blog(pool: web::Data<sqlx::Pool<Sqlite>>) -> Result<Vec<GetBlogs>, sqlx::Error> {
    let get_result = sqlx::query_as::<_, GetBlogs>("SELECT id, title, content, created_at FROM blog")
        .fetch_all(&**pool)
        .await?;
    Ok(get_result)
}
