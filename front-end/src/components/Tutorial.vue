<script setup>
import { ref, onMounted } from 'vue';

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false
  }
});

const emit = defineEmits(['close']);

const currentStep = ref(0);
const tutorialSteps = [
  {
    target: '.sidebar',
    title: 'Pamokų navigacija',
    content: 'Čia rasite visų kursų ir pamokų sąrašą. Pasirinkite pamoką norėdami pradėti mokytis.',
    position: 'right'
  },
  {
    target: '.content',
    title: 'Pamokų turinys',
    content: 'Šioje vietoje pateikiama pasirinktos pamokos teorinė medžiaga ir užduotys.',
    position: 'center'
  },
  {
    target: '.task',
    title: 'Praktinės užduotys',
    content: 'Kiekviena pamoka turi praktines užduotis, kurias galite atlikti ir įkelti savo sprendimus.',
    position: 'bottom'
  },
  {
    target: '.chat-toggle-button',
    title: 'Pagalbos asistentas',
    content: 'Turite klausimų? Čia galite bendrauti su AI asistentu, kuris padės išspręsti kilusius klausimus.',
    position: 'top-left'
  }
];

const targetPosition = ref({ top: 0, left: 0, width: 0, height: 0 });
const tooltipPosition = ref({ top: 0, left: 0 });

function calculatePosition() {
  if (currentStep.value >= tutorialSteps.length) return;
  
  const step = tutorialSteps[currentStep.value];
  setTimeout(() => {
    const element = document.querySelector(step.target);
    
    if (!element) {
      console.error(`Element with selector '${step.target}' not found`);
      return;
    }
    
    const rect = element.getBoundingClientRect();
    targetPosition.value = {
      top: rect.top,
      left: rect.left,
      width: rect.width,
      height: rect.height
    };
  
  switch (step.position) {
    case 'top':
      tooltipPosition.value = {
        top: rect.top - 120,
        left: rect.left + rect.width / 2 - 150
      };
      break;
    case 'right':
      tooltipPosition.value = {
        top: rect.top + rect.height / 2 - 60,
        left: rect.right + 20
      };
      break;
    case 'bottom':
      tooltipPosition.value = {
        top: rect.bottom + 20,
        left: rect.left + rect.width / 2 - 150
      };
      break;
    case 'left':
      tooltipPosition.value = {
        top: rect.top + rect.height / 2 - 60,
        left: rect.left - 320
      };
      break;
    case 'center':
      tooltipPosition.value = {
        top: rect.top + rect.height / 2 - 60,
        left: rect.left + rect.width / 2 - 150
      };
      break;
      case 'top-left':
      tooltipPosition.value = {
        top: rect.top + rect.height / 2 - 250,
        left: rect.left - 320
      };
      break;
  };
  }, 0);
}

function nextStep() {
  if (currentStep.value < tutorialSteps.length - 1) {
    currentStep.value++;
    calculatePosition();
  } else {
    completeTutorial();
  }
}

function prevStep() {
  if (currentStep.value > 0) {
    currentStep.value--;
    calculatePosition();
  }
}

function completeTutorial() {
  localStorage.setItem('tutorialCompleted', 'true');
  emit('close');
}

function skipTutorial() {
  emit('close');
}

onMounted(() => {
  if (props.isVisible) {
    calculatePosition();
    window.addEventListener('resize', calculatePosition);
  }
});
</script>

<template>
    <div v-if="isVisible" class="tutorial-overlay">
      <div class="overlay-background"></div>
      
      <div class="highlight-element" 
           :style="{
             top: `${targetPosition.top}px`,
             left: `${targetPosition.left}px`,
             width: `${targetPosition.width}px`,
             height: `${targetPosition.height}px`
           }">               
      </div>
          
      <div class="tooltip" 
           :style="{
             top: `${tooltipPosition.top}px`,
             left: `${tooltipPosition.left}px`
           }">
        <button @click="skipTutorial" class="close-button">×</button>
        
        <h3>{{ tutorialSteps[currentStep].title }}</h3>
        <p>{{ tutorialSteps[currentStep].content }}</p>
        
        <div class="tooltip-navigation">
          <button @click="prevStep" :disabled="currentStep === 0" class="nav-button">
            Atgal
          </button>
          <div class="step-indicator">
            {{ currentStep + 1 }} / {{ tutorialSteps.length }}
          </div>
          <button @click="nextStep" class="nav-button primary">
            {{ currentStep === tutorialSteps.length - 1 ? 'Baigti' : 'Toliau' }}
          </button>
        </div>
       
      </div>
    </div>
  </template>

<style scoped>
.tutorial-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: 9999;
  pointer-events: none;
}

.overlay-background {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.7);
  pointer-events: auto;
}

.tooltip {
  position: absolute;
  width: 300px;
  background-color: #2D2D2D;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  padding: 20px;
  z-index: 10001;
  pointer-events: auto;
  border-left: 4px solid #916ad5;
  color: white;
}

.tooltip h3 {
  margin-top: 0;
  font-size: 18px;
  color: #916ad5;
  margin-bottom: 10px;
}

.tooltip p {
  margin-bottom: 20px;
  font-size: 16px;
  line-height: 1.5;
}

.tooltip-navigation {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.nav-button {
  padding: 8px 15px;
  background-color: #777773;
  border: none;
  border-radius: 4px;
  color: white;
  cursor: pointer;
  font-size: 14px;
  transition: background-color 0.2s;
}

.nav-button:hover {
  background-color: #444;
}

.nav-button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.nav-button.primary {
  background-color: #916ad5;
  color: #000;
}

.nav-button.primary:hover {
  background-color: #9776d1;
}

.step-indicator {
  font-size: 14px;
  color: #ccc;
}

.close-button {
  position: absolute;
  top: 10px;
  right: 10px;
  width: 24px;
  height: 24px;
  background: none;
  border: none;
  font-size: 24px;
  line-height: 1;
  color: #777;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: color 0.2s;
  border-radius: 50%;
}

.close-button:hover {
  color: #916ad5;
}

.highlight-element {
  position: absolute;
  box-shadow: 0 0 0 9999px rgba(0, 0, 0, 0.7);
  border-radius: 4px;
  z-index: 10000;
  pointer-events: none;
  border: 2px solid #916ad5;
}
</style>