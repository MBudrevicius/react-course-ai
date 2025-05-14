<script setup>
import { ref, onMounted, watch } from "vue";
import UploadFile from '@/components/UploadFile.vue';

const props = defineProps({
  taskContent: String,
});

const isExpanded = ref(false);
const contentRef = ref(null);

watch(() => props.taskContent, () => {
  if (isExpanded.value && contentRef.value) {
    setTimeout(() => {
      contentRef.value.style.maxHeight = contentRef.value.scrollHeight + "px";
    }, 0);
  }
});

onMounted(() => {
  const collapsibles = document.querySelectorAll(".collapsible");
  
  collapsibles.forEach((btn) => {
    btn.addEventListener("click", function () {
      this.classList.toggle("active");
      isExpanded.value = this.classList.contains("active");
      const content = this.nextElementSibling;
      
      if (content.style.maxHeight) {
        content.style.maxHeight = null;
      } else {
        content.style.maxHeight = content.scrollHeight + "px";
      }
    });
  });
});
</script>

<template>
  <div class="task">
    <button class="collapsible">Užduotis</button>
    <div class="content" ref="contentRef">
      <p v-html="taskContent"></p>
      <UploadFile />
    </div>    
  </div>
</template>

<style scoped>
.collapsible {
  background-color: #2d2d2d;
  border-top: 2px solid #916ad5;
  color: white;
  cursor: pointer;
  padding: 5px;
  width: 100%;
  text-align: center;
  font-size: 20px;
  border-bottom-left-radius: 20px;
  border-bottom-right-radius: 20px;
}

.task {
  position: sticky;
  top: 0;
}

.active, .collapsible:hover {
  background-color: #916ad5;
}

.content {
  padding: 0 18px;
  max-height: 0;
  overflow: hidden;
  transition: max-height 0.2s ease-out;
  background-color: #2d2d2d;
  border-radius: 20px;
  justify-content: center;
  color: white;
}
@media (max-width: 768px) {
  .collapsible {
    font-size: 18px;
    padding: 8px 5px;
  }
  
  .content {
    padding: 0 10px;
  }
}

@media (max-width: 480px) {
  .collapsible {
    font-size: 16px;
  }
}
</style>