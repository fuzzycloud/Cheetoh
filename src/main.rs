use actix_web::{middleware, web, App, HttpServer};
use anyhow::Result;
use sqlx::migrate::MigrateDatabase;
use sqlx::sqlite::SqlitePoolOptions;
use sqlx::Sqlite;
use tera::Tera;
use actix_files as fs;
use crate::api::save_blog::save_blog;
use crate::helper::templates_dir;
use crate::pages::add_blog::add_blog;
use crate::pages::index::index;

mod helper;
mod pages;
mod api;

#[actix_web::main]
pub async fn main() -> Result<()> {
    env_logger::init_from_env(env_logger::Env::new().default_filter_or("info"));

    log::info!("starting HTTP server at http://localhost:5000");

    let db_conn = format!("sqlite://cheetoh.db");

    if !Sqlite::database_exists(&db_conn).await.unwrap_or(false) {
        println!("Creating database {}", db_conn);
        match Sqlite::create_database(&db_conn).await {
            Ok(_) => println!("Create db success for {}", db_conn),
            Err(error) => panic!("error: {}", error),
        }
    } else {
        println!("{} Database already exists", db_conn);
    }

    let pool = SqlitePoolOptions::new().connect(&db_conn).await?;

    sqlx::query(
        "CREATE TABLE IF NOT EXISTS blog 
        (   
            id TEXT PRIMARY KEY NOT NULL,
            title TEXT NOT NULL,
            content TEXT NOT NULL,
            created_at TIMESTAMP NOT NULL
        )",
    )
    .execute(&pool.clone())
    .await?;

   

    HttpServer::new(move || {
        let templates_dir = templates_dir().unwrap();
        let mut tera = Tera::new(&templates_dir).unwrap();
        tera.full_reload().unwrap();

        App::new()
            .app_data(web::Data::new(tera))
            .app_data(web::Data::new(pool.clone()))
            .wrap(middleware::Logger::default())
            .service(fs::Files::new("/static", "./static")
            .show_files_listing()
            .use_last_modified(true))
            .service(web::resource("/").route(web::get().to(index)))
            .service(web::resource("/add_blog")
                .route(web::get().to(add_blog))
                .route(web::post().to(save_blog))) 
            
            
    })
    .bind(("0.0.0.0", 5000))?
    .run()
    .await?;
    Ok(())
}
