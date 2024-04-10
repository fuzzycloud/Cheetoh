use actix_web::{error, web, Error, Responder};
use actix_web_lab::respond::Html;

pub async fn add_blog(tmpl: web::Data<tera::Tera>) -> Result<impl Responder, Error> {
    let s = tmpl
        .render("add_blog.html", &tera::Context::new())
        .map_err(|e| {
            println!("{:#?}", e);
            error::ErrorInternalServerError("Template error for blog")
        })?;
    Ok(Html(s))
}
