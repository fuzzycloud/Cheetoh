use std::env::current_dir;
use anyhow::Result;

pub fn templates_dir() -> Result<String>  {
    let dir = current_dir()?;
    Ok(format!("{}/templates/**/*.html", dir.display()))
}