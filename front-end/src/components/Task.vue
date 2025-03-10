<script setup>
import { ref, onMounted } from "vue";

const collapsible = ref(null);

onMounted(() => {
  const collapsibles = document.querySelectorAll(".collapsible");
  
  collapsibles.forEach((btn) => {
    btn.addEventListener("click", function () {
      this.classList.toggle("active");
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
      <!-- Čia turėtų vykti užduoties mappinimas -->
      <div class="content">
          <p>Atvaizduoti tekstą "Hello, World!" naudojant React komponentą.</p>
      </div>
    </div>
</template>

<style scoped>
.collapsible {
  background-color: #2D2D2D;
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
  background-color: #2D2D2D;
  border-radius: 20px;
  justify-content: center;
}
</style>