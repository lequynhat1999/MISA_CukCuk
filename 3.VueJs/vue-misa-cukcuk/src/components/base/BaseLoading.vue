<template>
  <div class="bg-loading" :class="{loading:isLoading}">
    <div class="icon-loader" :class="{loader:isLoading}"></div>
  </div>
</template>

<script>
export default {
  name: "BaseLoading",
  props: ["isLoading"],
};
</script>

<style>
.loading {
  position: fixed;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  background-color: #737373;
  opacity: 0.6;
}
.loader {
  position: absolute;
  top: 50%;
  left: 50%;
  border: 8px solid #f3f3f3;
  border-radius: 50%;
  border-top: 8px solid gray;
  width: 30px;
  height: 30px;
  -webkit-animation: spin 2s linear infinite; /* Safari */
  animation: spin 2s linear infinite;
}

/* Safari */
@-webkit-keyframes spin {
  0% {
    -webkit-transform: rotate(0deg);
  }
  100% {
    -webkit-transform: rotate(360deg);
  }
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>