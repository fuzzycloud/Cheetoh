use leptos::*;

// Renders the home page of your application.
#[component]
pub fn HomePage() -> impl IntoView {
    let (count, set_count) = create_signal(0);

    view! {
        <main class="my-0 mx-auto max-w-3xl text-center">
            <h2 class="p-6 text-4xl">"Welcome to Leptos with Tailwind"</h2>
            <p class="px-10 pb-10 text-left">
                "Tailwind will scan your Rust files for Tailwind class names and compile them into a CSS file."
            </p>
            <button class="btn btn-neutral" on:click=move |_| set_count.update(|count| *count += 1)>
                "Something's here | "
                {move || {
                    if count.get() == 0 { "Click me!".to_string() } else { count.get().to_string() }
                }}

                " | Some more text"
            </button>
        </main>
    }
}
