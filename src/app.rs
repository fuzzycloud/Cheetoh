use crate::pages::{
    home_page::HomePage, login_page::LoginPage, not_found::NotFound, signup_page::SignUpPage,
    write_blog::WriteBlog,
};
use leptos::*;
use leptos_meta::*;
use leptos_router::*;

#[component]
pub fn App() -> impl IntoView {
    // Provides context that manages stylesheets, titles, meta tags, etc.
    provide_meta_context();
    view! {
        <Stylesheet id="leptos" href="/pkg/cheetoh.css"/>
        <Link rel="shortcut icon" type_="image/ico" href="/favicon.ico"/>
        // sets the document title
        <Title text="Welcome to Leptos"/>
        // content for this welcome page
        <Router>
            <main>
                <Routes>
                    <Route path="/" view=LoginPage/>
                    <Route path="/write-blog" view=WriteBlog/>
                    <Route path="/signup" view=SignUpPage/>
                    <Route path="/home_page" view=HomePage/>
                    <Route path="/*any" view=NotFound/>
                </Routes>
            </main>
        </Router>
    }
}
