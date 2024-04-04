#[cfg(feature = "ssr")]
#[actix_web::main]
async fn main() -> std::io::Result<()> {
    use actix_files::Files;
    use actix_web::{middleware::Logger, *};
    use cheetoh::{
        app::*,
        dto::app_state::AppState,
        helper::{login::login, singup::signup},
    };
    use dotenv::var;
    use leptos::*;
    use leptos_actix::{generate_route_list, LeptosRoutes};
    use log::info;
    use sqlx::{migrate::MigrateDatabase, sqlite::SqlitePoolOptions, Sqlite};

    let conf = get_configuration(None).await.unwrap();
    let addr = conf.leptos_options.site_addr;
    // Generate the list of routes in your Leptos App
    let routes = generate_route_list(App);
    println!("listening on http://{}", &addr);

    std::env::set_var("RUST_LOG", "debug");
    std::env::set_var("RUST_BACKTRACE", "1");
    env_logger::init_from_env(env_logger::Env::new().default_filter_or("info"));
    dotenv::dotenv().ok();

    let read_dev_db = var("READ_DB_FILE").expect("READ DB name must be set");
    let read_db_conn = format!("sqlite://{}", &read_dev_db);

    if !Sqlite::database_exists(&read_db_conn)
        .await
        .unwrap_or(false)
    {
        info!("Creating database {}", read_db_conn);
        match Sqlite::create_database(&read_dev_db).await {
            Ok(_) => info!("Create db success"),
            Err(error) => panic!("error: {}", error),
        }
    } else {
        info!("{} Database already exists", read_db_conn);
    }

    let read_pool = SqlitePoolOptions::new()
        .connect(&format!("sqlite:{}", &read_dev_db))
        .await
        .expect("Failed to connect to the database");

    sqlx::query(
        r#"
        CREATE TABLE IF NOT EXISTS auth (
            id TEXT PRIMARY KEY,
            email TEXT NOT NULL,
            password TEXT NOT NULL
        )
        "#,
    )
    .execute(&read_pool)
    .await
    .expect("Failed to create table");

    HttpServer::new(move || {
        let leptos_options = &conf.leptos_options;
        let site_root = &leptos_options.site_root;

        let app_state = AppState {
            read_pool: read_pool.clone(),
        };

        App::new()
            .app_data(web::Data::new(app_state))
            .app_data(web::Data::new(read_pool.clone()))
            .wrap(Logger::new("%a %{User-Agent}i - %D millisecond"))
            .wrap(middleware::Compress::default())
            // serve JS/WASM/CSS from `pkg`
            .service(Files::new("/pkg", format!("{site_root}/pkg")))
            // serve other assets from the `assets` directory
            .service(Files::new("/assets", site_root))
            // serve the favicon from /favicon.ico
            .service(favicon)
            .leptos_routes(leptos_options.to_owned(), routes.to_owned(), App)
            .app_data(web::Data::new(leptos_options.to_owned()))
            .service(signup)
            .service(login)
        //.wrap(middleware::Compress::default())
    })
    .bind(&addr)?
    .run()
    .await
}

#[cfg(feature = "ssr")]
#[actix_web::get("favicon.ico")]
async fn favicon(
    leptos_options: actix_web::web::Data<leptos::LeptosOptions>,
) -> actix_web::Result<actix_files::NamedFile> {
    let leptos_options = leptos_options.into_inner();
    let site_root = &leptos_options.site_root;
    Ok(actix_files::NamedFile::open(format!(
        "{site_root}/favicon.ico"
    ))?)
}

#[cfg(not(any(feature = "ssr", feature = "csr")))]
pub fn main() {
    // no client-side main function
    // unless we want this to work with e.g., Trunk for pure client-side testing
    // see lib.rs for hydration function instead
    // see optional feature `csr` instead
}

#[cfg(all(not(feature = "ssr"), feature = "csr"))]
pub fn main() {
    // a client-side main function is required for using `trunk serve`
    // prefer using `cargo leptos serve` instead
    // to run: `trunk serve --open --features csr`
    use cheetoh::app::*;
    use leptos::*;
    use wasm_bindgen::prelude::wasm_bindgen;

    console_error_panic_hook::set_once();

    leptos::mount_to_body(App);
}
