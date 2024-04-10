use actix_web::{web, Error};
use actix_web_lab::respond::Html;
use sqlx::Sqlite;
use tera::Context;

use crate::api::get_blog::get_blog;

pub async fn index(tmpl: web::Data<tera::Tera>, pool: web::Data<sqlx::Pool<Sqlite>>) -> Result<impl actix_web::Responder, Error> {
    let mut context = Context::new();

    let blogs = match get_blog(pool).await {
        Ok(blogs) => blogs,
        Err(_) => return Err(actix_web::error::ErrorInternalServerError("Failed to fetch blogs")),
    };

    context.insert("blogs", &blogs);

    let rendered = tmpl
        .render("index.html", &context)
        .map_err(|e| {
            println!("{:#?}", e);
            actix_web::error::ErrorInternalServerError("Template error for home")
        })?;

    Ok(Html(rendered))
}
