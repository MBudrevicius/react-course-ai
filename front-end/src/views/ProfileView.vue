<script setup>
import { ref, onMounted } from 'vue';
import Navbar from '../components/Navbar.vue';
import SpinningLoader from '@/components/SpinningLoader.vue';
import VueApexCharts from "vue3-apexcharts";

onMounted(async () => {
  try {
    const data = await getSolutions();
    console.log("111:" + data);
  } catch (error) {}
})
const loading = ref(false);

const series = [20, 80];

const options = {
  legend: {
    show: false
},
  chart: {
    type: 'donut'
  },
  plotOptions: {
    pie: {
      donut: {
        labels: {
          show: false,
          name: {
            show: false  // Hide the name label
          },
          value: {
            show: true,
            fontSize: '24px',
            color: '#fff' // Optional: white text
          }
        }
      }
    }
  },
  colors:['#916ad5', '#333333'],
  dataLabels: {
    enabled: false  // Hide slice labels outside the donut
  }
};

</script>

<template>
  <Navbar />
  <SpinningLoader v-if="loading" />
  <div class="container">
    <h1>Pažangumas</h1>
    <p>Įvykdei 20% užduočių</p>
    <VueApexCharts width="300" type="donut" :options="options" :series="series" />
    <p>Vidurkis: 4,5</p>
    <div class="container">
        <h1>Tau puikiai sekėsi atlikti šias užduotis:</h1>
        <p>Įvadas į React</p>
    </div>
    <div class="container">
        <h1>Dar reikia pasistengti šiose temose:</h1>
        <p>Komponentai ir JSX</p>
        <p>Formos ir vartotojo įvestis</p>
    </div>
  </div>
</template>

<style scoped>
p {
  font-size: 1.5rem;
  margin-bottom: 1rem;
  color: white;
}
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
