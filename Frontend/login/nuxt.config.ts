// https://nuxt.com/docs/api/configuration/nuxt-config


import { defineConfig } from 'vite'
import tailwindcss from '@tailwindcss/vite'
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
    modules: [
    '@pinia/nuxt'
  ],
  devtools: { enabled: true },

  vite:{
    plugins:[
      tailwindcss()
    ]
  }
})
