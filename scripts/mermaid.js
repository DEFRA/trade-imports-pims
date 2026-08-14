if (typeof mermaid !== "undefined") {
  mermaid.initialize({
    startOnLoad: true,
    theme: "default",
    flowchart: {
      defaultRenderer: "elk",
      nodeSpacing: 50,
      rankSpacing: 80,
      useMaxWidth: false
    }
  });
} else {
  console.warn("Mermaid library not loaded; skipping diagram initialization.");
}