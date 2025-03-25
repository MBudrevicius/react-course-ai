<script setup>
import { defineEmits, defineProps, ref, onMounted, watch } from 'vue';
import { getTasksByLessonId, getBestSubmissionByProblemId } from '../api/lessonAPI';
import { useRoute } from 'vue-router';

const route = useRoute();
const emit = defineEmits(['close']);
const submission = ref('');
const lessonId = ref(route.params.id);
const taskContent = ref('No tasks available')
const taskId = ref('');

onMounted(async () => {
    await fetchTasks();
    await fetchSubmission();
})

// watch(() => route.params.id, async (newId) => {
//     lessonId.value = newId;
//     fetchSubmission();
//     fetchTasks();
// });

async function fetchTasks() {
  try {
    const tasks = await getTasksByLessonId(lessonId.value);
    taskContent.value = tasks.length > 0 ? tasks[0].question : 'No tasks available';
    console.log('Tasks:', tasks);
    taskId.value = tasks[0].id;
  } catch (error) {
    console.log('Error fetching tasks:', error);
    taskContent.value = 'Error fetching tasks';
  }
}

async function fetchSubmission() {
  try {
    submission.value = await getBestSubmissionByProblemId(taskId.value);
    console.log(submission);
    // submissionScore.Score = submissions.Score > 0 ? submissions[0].Score : 'Jūs dar nepateikėte sprendimo šiai užduočiai';
  } catch (error) {
    console.log('Error fetching submissions:', error);
    // submissionScore.value = 'Error fetching submissions';
  }
}

const props = defineProps({
    mode: String
})

function closeModal() {
    emit('close');
}

function downloadSubmission(code) {
    console.log(code);
    const blob = new Blob([code], { type: 'application/javascript' });

    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = 'script.js'; // Name of the file

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
</script>

<template>
    <div class="modal">
        <div class="modal-content">
            <span class="close-btn" @click="closeModal">&times;</span>

            <template v-if="props.mode === 'submit'">
                <h2>Sveikiname! Sėkmingai įvykdėte užduotį!</h2>
                <p>Kur galėtumėte patobulėti:</p>
                <li>Sintaksė: 100 taškų</li>
                <li>Kodo teisingumas: 100 taškų</li>
                <li>Užduoties taisyklingumas: 100 taškų</li>
                <p style="font-size: 18px; margin-top: 5px;">Galutinis rezultatas:</p>
                <div class="progress-container">
                    <div class="progress-bar"></div>
                    <span class="percentage">100%</span>
                </div>
            </template>
            
            <template v-if="props.mode === 'best-solution'">
                <h2>Geriausias sprendimas</h2>
                <p style="font-size: 18px; margin-top: 5px;">Rezultatas:</p>
                <div class="progress-container">
                    <div class="progress-bar"></div>
                    <span class="percentage">{{submission.score}}%</span>
                </div>
                <p style="font-size: 18px; margin-top: 5px;">Pastabos:</p>
                <p style="font-size: 18px; margin-top: 5px; font-style: italic;">{{submission.feedback}}</p>
                <button @click="downloadSubmission(submission.code)">Atsisiųsti</button>
            </template>
        </div>
    </div>
</template>

<style scoped>
.progress-container {
    display: flex;
    align-items: center;
    gap: 10px;
}

.percentage {
    font-size: 20px;
    color: white;
    min-width: 50px;
    text-align: right;
    display: flex;
    margin-top: 10px; 
}

.progress-bar {
    align-items: center;
    background: #916ad5;
    width: 0%;
    height: 20px;
    border-radius: 20px;
    transition: 0.5s linear;
    transition-property: width, background-color;
    animation: progress 5s forwards;
    margin-top: 10px;
}

@keyframes progress {
    from {
        width: 0%;
        background-color: #eb4034;
    }
    to {
        width: 100%;
        background-color: #22992c;
    }
}

h2 {
    color: white;
    font-size: 25px;
    text-align: center;
    margin-bottom: 20px;
    border-bottom: #4E4E4E 2px solid;
}

li {
    color: white;
    font-size: 20px;
    margin-left: 40px;
}

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
    color: #916ad5;
}

button {
    border-radius: 25px;
    background-color: #1c1c1c;
    color: #916ad5;
    padding: 5px;
    cursor: pointer;
    font-size: 20px;
    padding-left: 15px;
    padding-right: 15px;
    margin-top: 15px;
    display: block;
    margin-left: auto;
    margin-right: auto;
}

</style>
