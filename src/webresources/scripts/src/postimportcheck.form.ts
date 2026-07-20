namespace DefraImports.PostImportCheck {

  let formContext: Form.defraimp_importinspection.Main.Information;

  export function onLoad(executionObj: Xrm.ExecutionContext<any>) {
    formContext = executionObj.getFormContext() as Form.defraimp_importinspection.Main.Information;
    showHideSampleTestsBasedOnSamplingRequired();
  }

  export function onChangeSamplingRequired() {
    showHideSampleTestsBasedOnSamplingRequired();
  }

  function showHideSampleTestsBasedOnSamplingRequired() {
    const isSamplingRequired: boolean = formContext.getAttribute("defraimp_samplingrequired").getValue();
    const sampleTestsRequiredSubgrid: Xrm.SubGridControl<any> = formContext.getControl("SamplesTestsRequired");

    if (isSamplingRequired) {
      sampleTestsRequiredSubgrid.setVisible(true);
    }
    else {
      sampleTestsRequiredSubgrid.setVisible(false);
    }
  }
}