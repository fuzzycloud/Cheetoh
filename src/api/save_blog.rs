use actix_web::{web, Responder, HttpResponse};
use sqlx::SqlitePool;
use serde::{Deserialize, Serialize};
use uuid::Uuid;

#[derive(Deserialize, Serialize)]
pub struct FormData {
    title: String,
    content: String,
}

pub async fn save_blog(data: web::Json<FormData>, pool: web::Data<SqlitePool>) -> impl Responder {
    println!("Received title: {}", data.title);
    println!("Received content: {}", data.content);
    
    let mut tx = match pool.begin().await {
        Ok(tx) => tx,
        Err(e) => {
            eprintln!("Failed to start transaction: {:?}", e);
            return HttpResponse::InternalServerError().finish();
        }
    };

    match sqlx::query(
        "INSERT INTO blog (id, title, content, created_at) VALUES ($1, $2, $3, CURRENT_TIMESTAMP)"
    )
    .bind(Uuid::new_v4().to_string())
    .bind(&data.title)
    .bind(&data.content)
    .execute(&mut *tx)
    .await {
        Ok(_) => {
            match tx.commit().await {
                Ok(_) => {
                    println!("Transaction committed successfully!");
                    HttpResponse::Ok().finish()
                },
                Err(e) => {
                    eprintln!("Failed to commit transaction: {:?}", e);
                    HttpResponse::InternalServerError().finish()
                }
            }
        },
        Err(e) => {
            eprintln!("Failed to execute query: {:?}", e);
            match tx.rollback().await {
                Ok(_) => {
                    println!("Transaction rolled back!");
                    HttpResponse::InternalServerError().finish()
                },
                Err(e) => {
                    eprintln!("Failed to rollback transaction: {:?}", e);
                    HttpResponse::InternalServerError().finish()
                }
            }
        }
    }
}