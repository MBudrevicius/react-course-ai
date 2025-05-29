<script setup>
import loader from '@/assets/spin-loader.gif';
</script>

<template>
    <div class="loader-container">
        <img
            :src="loader"
            alt="Loading..."
            class="loader-image">
    </div>
</template>

<style scoped>
.loader-container {
    position: fixed;
    top: 0;
    left: 0;
    height: 100vh;
    width: 100vw;
    background-color: rgba(45, 45, 45, 0.8);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;  
}
</style>