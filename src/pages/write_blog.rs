
use leptos::*;

#[component]
pub fn WriteBlog() -> impl IntoView {
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
                        <a href="#" class="text-black font-semibold hover:text-gray-200">Add Blog</a>
                    </div>
                </div>
            </nav>

            <form class="space-y-6" action="/blog" method="post">
           
            <div class="flex flex-col items-center mt-8">
                <div class="max-w-5xl w-full mx-auto">
                    <input type="title" id= "title" placeholder="Enter Blog Title" name="title" class="w-full font-bold text-5xl px-3 py-4 mb-4 text-black bg-gray-100 border-b-2 border-black focus:outline-none focus:border-black"/>

                    <textarea rows="40" id="content" name="content" class="w-full px-3 py-2 mb-4 text-black text-3xl bg-white border border-black rounded-lg focus:outline-none focus:border-black" placeholder="Write your blog content here..."></textarea>

                    <button class="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline">Publish</button>
                </div>
            </div>
            </form >
        </div>
    }
}
