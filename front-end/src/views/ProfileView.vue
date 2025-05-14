<script setup>
import Navbar from '../components/Navbar.vue';
import SpinningLoader from '@/components/SpinningLoader.vue';
</script>

<template>
    <Navbar />
    <SpinningLoader v-if="loading" />
    <div class="container">
        <h1>Tavo pažangumas</h1>
        <p>This is the profile view.</p>
        <button @click="loading = !loading">Toggle Loading</button>
    </div>
</template>

<style scoped>
h1 {
    font-size: 2rem;
    margin-bottom: 1rem;
    color: white;
}
.container {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    padding: 20px;
}
</style>