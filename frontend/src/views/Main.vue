<template>
  <div>
    <headerbar/>
    <gradform/>
  </div>
</template>

<script>
import headerbar from "@/components/HeaderBar";
import gradform from "@/components/GradForm";
export default {
  name: "mainview",
  components: {
    headerbar,
    gradform
  }
};
</script>
