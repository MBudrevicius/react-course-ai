<template>
  <div class="container">
    <h1>Greitai čia atsiras išsamūs React programavimo kursai!</h1>
    <p class="subtext">Ko gali tikėtis ateityje:</p>

    <div class="screenshots">
      <img v-for="(image, index) in images" :key="index" :src="image" :alt="'Svetainės dizainas ' + (index + 1)" />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const images = ref([
  './src/assets/prototype_images/image1.png',
  './src/assets/prototype_images/image2.png',
  './src/assets/prototype_images/image3.png',
  './src/assets/prototype_images/image4.png'
]);
</script>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  text-align: center;
}

h1 {
  font-size: 2rem;
  font-weight: bold;
  margin-bottom: 10px;
}

.subtext {
  font-size: 1.2rem;
  color: #666;
  margin-bottom: 30px;
}

.screenshots {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 30px;
}

.screenshots img {
  width: 100%;
  max-width: 900px;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}
</style>