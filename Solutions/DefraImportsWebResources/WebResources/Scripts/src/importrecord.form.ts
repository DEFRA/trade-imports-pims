namespace DefraImports.ImportRecord {

    export function OnLoadQuickCreateForm(executionObj: Xrm.ExecutionContext<any>) {

        let formContext = executionObj.getFormContext() as Form.defraimp_importapplication.Quick.Information;

        if (formContext.ui.getFormType() === Xrm.FormType.Create) {
            formContext.getAttribute("ownerid").setValue(null);
        }
    }
}