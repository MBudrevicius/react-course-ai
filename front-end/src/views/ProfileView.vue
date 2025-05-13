<script setup>
import Navbar from '../components/Navbar.vue';
import SpinningLoader from '@/components/SpinningLoader.vue';
</script>

<template>
    <Navbar />
    <SpinningLoader v-if="loading" />
    <p>wow</p>
</template>

<style scoped>

</style>