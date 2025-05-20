<script setup>
import { ref, onMounted } from 'vue';
import Navbar from '../components/Navbar.vue';
import SpinningLoader from '@/components/SpinningLoader.vue';
import VueApexCharts from "vue3-apexcharts";
import { getSolutions } from '@/api/userAPI';

const loading = ref(true);
const series = ref([0, 0]);
const options = ref({
  tooltip: {
    enabled: false
  },
  states: {
    hover: {
      filter: {
        type: 'none'
      }
    },
    active: {
      filter: {
        type: 'none'
      }
    }
  },
  stroke: {
    width: 0
  },
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
            show: false
          },
          value: {
            show: true,
            fontSize: '24px',
            color: '#fff'
          }
        }
      }
    }
  },
  colors:['#916ad5', '#333333'],
  dataLabels: {
    enabled: false
  }
});

const successfulLessons = ref([]);
const lessonsNeedingImprovement = ref([]);
const averageScore = ref(0);

onMounted(async () => {
  try {
    const data = await getSolutions();
    const scores = data.map(item => item.score);
    const totalLessons = scores.length;
    const completedLessons = data.filter(item => item.score >= 50);
    const percentComplete = (completedLessons.length / totalLessons) * 100;

    series.value = [percentComplete, 100 - percentComplete];

    successfulLessons.value = completedLessons.filter(item => item.score > 80).map(item => item.lessonTitle);
    lessonsNeedingImprovement.value = data.filter(item => item.score < 50).map(item => item.lessonTitle);

    const totalScore = scores.reduce((sum, val) => sum + val, 0);
    averageScore.value = (totalScore / totalLessons).toFixed(1);

  } catch (error) {
  } finally {
    loading.value = false;
  }
});
</script>

<template>
  <Navbar />
  <SpinningLoader v-if="loading" />
  <div style=" display: flex; align-items: center; justify-content: center; flex-direction: column;">
    <div class="container">
      <h1>Įvykdei {{ series[0] }}% užduočių</h1>
      <VueApexCharts width="300" type="donut" :options="options" :series="series" />
      <p>Vidutinis įvertinimas: {{ averageScore }}%</p>
        <div class="container">
          <h1>Tau puikiai sekėsi atlikti šias užduotis:</h1>
          <p class="explanation">Užduoties sprendimo įvertinimas buvo 80% ir daugiau</p>
          <p v-if="successfulLessons.length === 0">Nėra užduočių su pažanga.</p>
          <li v-for="lesson in successfulLessons" :key="lesson">{{ lesson }};</li>
        </div>

        <div class="container">
          <h1>Dar reikia pasistengti šiose temose:</h1>
          <p class="explanation">Neišspręstos užduotys</p>
          <p v-if="lessonsNeedingImprovement.length === 0">Nėra neišspręstų užduočių.</p>
          <li v-for="lesson in lessonsNeedingImprovement" :key="lesson">{{ lesson }}</li>
        </div>
    </div>
  </div>
</template>

<style scoped>
.explanation {
  margin-bottom: 1rem;
  font-size: 1.2rem;
  color: #916ad5;
}
li {
  font-size: 1.5rem;
  color: white;
}
p {
  font-size: 1.5rem;
  margin-bottom: 1rem;
  color: white;
}
h1 {
  font-size: 2rem;
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
