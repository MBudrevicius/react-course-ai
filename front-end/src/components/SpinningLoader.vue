<template>
    <div class="loader-container">
        <div class="loader"></div>
    </div>
</template>

<style scoped>
.loader {
  width: 50px;
  aspect-ratio: 1;
  color: #854f1d;
  border-radius: 50%;
  display: grid;
  background: 
    conic-gradient(from 90deg at 4px 4px,#0000 90deg,currentColor 0)
    -4px -4px/calc(50% + 2px) calc(50% + 2px),
    radial-gradient(farthest-side,currentColor 6px,#0000 7px calc(100% - 6px),currentColor calc(100% - 5px)) no-repeat;
  animation: l10 2s infinite linear;
  position: relative;
}
.loader:before {
  content: "";
  border-radius: inherit;
  background: inherit;
  transform: rotate(45deg);
}
@keyframes l10 {to{transform: rotate(.5turn)}}


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