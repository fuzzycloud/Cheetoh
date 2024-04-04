#[derive(Clone)]
#[cfg(feature = "ssr")]
pub struct AppState {
    pub read_pool: sqlx::Pool<sqlx::Sqlite>,
}
