<template>
    <div class="bg-white py-24">
        <div class="flex flex-col items-center">
            <h1 class="text-6xl font-semibold text-blue-600">{{ title }}</h1>
            <p class="mt-4 text-9xl font-bold text-gray-900">Demo</p>
        </div>
    </div>
</template>

<script lang="ts" setup>
const year: string = '2022'
const title: string = `${year} iThome 鐵人賽`
</script>
