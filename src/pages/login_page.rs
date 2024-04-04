use leptos::*;

#[component]
pub fn LoginPage() -> impl IntoView {
    view! {
        <div class="min-h-screen bg-gray-100 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
            <div class="sm:mx-auto sm:w-full sm:max-w-md mb-8">
                <h1 class="text-center text-6xl font-extrabold text-blue-900">Cheetoh</h1>
            </div>

            <div class="sm:mx-auto sm:w-full sm:max-w-md">
                <h2 class="text-center text-3xl font-extrabold text-gray-900">Log in to your account</h2>
            </div>
            <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
                <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
                    <form class="space-y-6" action="/" method="POST">
                        <div>
                            <label for="login_email" class="block text-sm font-medium text-gray-700">Email address</label>
                            <input id="login_email" name="email" type="email" required class="darken-input mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md"/>
                        </div>

                        <div>
                            <label for="login_password" class="block text-sm font-medium text-gray-700">Password</label>
                            <input id="login_password" name="password" type="password" required class="darken-input mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md"/>
                        </div>

                        <div class="flex items-center justify-between">
                            <div class="text-sm">
                                <a href="#" class="font-medium text-indigo-600 hover:text-indigo-500">Forgot your password?</a>
                            </div>
                        </div>

                        <div>
                            <button type="submit" class="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
                                Login
                            </button>
                        </div>
                    </form>
                </div>
            </div>

            <div class="mt-6">
                <p class="mt-2 text-center text-sm text-gray-600">
                    Don 't have an account? <a href="/signup" class="font-medium text-indigo-600 hover:text-indigo-500">Sign up</a>
                </p>
            </div>
        </div>
    }
}
