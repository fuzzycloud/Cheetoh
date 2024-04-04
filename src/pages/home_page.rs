use leptos::*;

#[component]
pub fn HomePage() -> impl IntoView {

    view! {
        <div>
            <nav class="flex items-center justify-between p-6 bg-blue-500 text-black">
                <div class="flex items-center flex-shrink-0 mr-6 text-3xl">
                    <span class="font-semibold tracking-tight">Cheetoh</span>
                </div>
                <div class="hidden lg:block">
                    <div class="flex items-center text-2xl">
                        <a href="#" class="mr-4 font-semibold text-black hover:text-black-200">Contact Us</a>
                        <a href="#" class="mr-4 font-semibold text-black hover:text-black-200">About Us</a>
                        <a href="/write-blog" class="text-black font-semibold hover:text-gray-200">Add Blog</a>
                    </div>
                </div>
            </nav>
            <div class="flex justify-center mt-8">
                <a href="/write-blog">
                    <button class="bg-black hover:bg-white-800 text-white font-bold py-2 px-4 rounded">
                        "Add Blog"
                    </button>
                </a>
            </div>
        </div>
    }
}
