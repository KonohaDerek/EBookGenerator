/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './src/components/**/*.{vue,js,ts}',
        './src/layouts/**/*.vue',
        './src/pages/**/*.vue',
        './src/composables/**/*.{js,ts}',
        './src/plugins/**/*.{js,ts}',
        './src/pages/index.{js,ts,vue}'
    ],
    theme: {
        extend: {}
    },
    plugins: []
}
