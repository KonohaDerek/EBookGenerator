// https://v3.nuxtjs.org/api/configuration/nuxt.config
export default defineNuxtConfig({
    ssr: false,
    srcDir: 'src/',
    modules: ['@nuxtjs/tailwindcss'],
    typescript: {
        typeCheck: true,
        shim: false
    }
})
