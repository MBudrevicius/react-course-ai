<script setup>
import { defineEmits, defineProps, ref, onMounted } from 'vue';
import { getTasksByLessonId, getBestSubmissionByProblemId } from '@/api/problemAPI';
import { useRoute } from 'vue-router';

const emit = defineEmits(['close']);
const route = useRoute();
const lessonId = ref(route.params.id);
const taskId = ref('');
const submission = ref('');

const props = defineProps({
  mode: String,
  evaluationResult: Object
});

const score = ref(0);
const feedback = ref('');
const code = ref('');
const statusTitle = ref('');
const barId = ref('score-bar');
const scoreId = ref('score-text');

onMounted(async () => {
  if (props.mode === 'best-solution') {
    await fetchTasks();
    await fetchSubmission();
    score.value = submission.value.score;
    feedback.value = submission.value.feedback;
    code.value = submission.value.code;
  } else {
    score.value = props.evaluationResult.score;
    feedback.value = props.evaluationResult.feedback;
  }
  setStyle();
})

async function fetchTasks() {
  try {
    const tasks = await getTasksByLessonId(lessonId.value);
    taskId.value = tasks[0].id;
  } catch (error) {}
}

async function fetchSubmission() {
  try {
    submission.value = await getBestSubmissionByProblemId(taskId.value);
  } catch (error) {}
}

function setStyle() {
  const progressBar = document.getElementById(barId.value);
  const scoreElement = document.getElementById(scoreId.value);

  setTimeout(() => {
    progressBar.style.width = score.value + '%';
  }, 100);

  if (score.value >= 50) {
    progressBar.style.background = '#22992c';
    scoreElement.style.color = '#22992c';
    statusTitle.value = 'Užduotis įvykdyta!';
  } else {
    progressBar.style.background = '#eb4034';
    scoreElement.style.color = '#eb4034';
    statusTitle.value = 'Užduoties atlikti nepavyko :(';
  }

  if (score.value >= 90) {
    statusTitle.value = 'Užduotis atlikta puikiai!';
  }

  let currentScore = 0;
  const interval = setInterval(() => {
    if (currentScore < score.value) {
      currentScore++;
      scoreElement.innerText = `${currentScore}%`;
    } else {
      clearInterval(interval);
    }
  }, 1900 / score.value);
}

function closeModal() {
  emit('close');
}

function downloadSubmission(codeContent) {
  const blob = new Blob([codeContent], { type: 'application/javascript' });
  const link = document.createElement('a');
  link.href = URL.createObjectURL(blob);
  link.download = 'script.js';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}
</script>

<template>
  <div class="modal">
    <div class="modal-content">
      <span class="close-btn" @click="closeModal">&times;</span>
      <h2 :class="score >= 90 ? 'perfect' : score >= 50 ? 'success' : 'fail'">
        {{ statusTitle }}
      </h2>
      <p class="label">Rezultatas:</p>
      <div class="progress-container">
        <div class="progress-bar" :id="barId"></div>
        <span class="percentage" :id="scoreId">0%</span>
      </div>

      <p class="label" style="margin-top: 15px;">Atsiliepimas:</p>
      <p class="feedback">{{ feedback }}</p>

      <button v-if="props.mode === 'best-solution'" @click="downloadSubmission(code)">
        Atsisiųsti sprendimą
      </button>
    </div>
  </div>
</template>

<style scoped>
.modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999;
}

.modal-content {
    background: #2D2D2D;
    padding: 20px;
    border-radius: 10px;
    width: 700px;
    position: relative;
}

.close-btn {
    position: absolute;
    top: 5px;
    right: 15px;
    font-size: 30px;
    cursor: pointer;
}

h2 {
    font-size: 25px;
    text-align: center;
    margin-bottom: 10px;
    border-bottom: #4E4E4E 2px solid;
    font-weight: bold;
}

.success {
    color: #4caf50;
}

.fail {
    color: #f44336;
}

.perfect {
    color: #4caf50;
    animation: glow 1.5s infinite alternate;
}

@keyframes glow {
  from {
    text-shadow: 0 0 5px #4caf50;
  }
  to {
    text-shadow: 0 0 20px #4caf50;
  }
}

.label {
    font-size: 18px;
    margin-top: 5px;
    font-weight: bold;
}

.progress-container {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-bottom: 10px;
}

.progress-bar {
    align-items: center;
    background: #eb4034;
    width: 0px;
    height: 20px;
    border-radius: 20px;
    transition: 1s linear;
    transition-property: width, background-color;
    transition: width 3.5s ease-in-out, background-color 3.5s ease-in-out;
}

.percentage {
    color: #f44336;
    font-weight: bold;
    min-width: 50px;
    text-align: right;
    transition: color 3.5s ease-in-out;
}

.feedback {
    font-size: 16px;
    font-style: italic;
    background: #3a3a3a;
    padding: 10px;
    border-radius: 8px;
}

button {
    margin-top: 20px;
    display: block;
    margin-left: auto;
    margin-right: auto;
    padding: 10px 20px;
    border: none;
    border-radius: 25px;
    background-color: #444;
    color: #fff;
    font-size: 16px;
    cursor: pointer;
    transition: background-color 0.3s;
    font-weight: bold;
}

button:hover {
    background-color: #666;
}

.perfect-score {
    background: #4caf50 !important;
    animation-delay: 3.5s;
}
</style>
