namespace DefraImports.ImportQuery {

    export function CloneImportQueryButton(primaryControl: Form.defraimp_importquery.Main.Information) {
        console.log("Function called!");
        const emailToSend = primaryControl.getAttribute("defraimp_querysentto").getValue();
        console.log(emailToSend);
        const subject = primaryControl.getAttribute("subject").getValue();
        console.log(subject);
        const dueDate = primaryControl.getAttribute("defraimp_duedate").getValue();
        console.log(dueDate?.toString());
        const queryId = primaryControl.data.entity.getId();
        console.log(queryId);

        let clone: any = {};
        clone["defraimp_querysentto"] = emailToSend;
        clone["subject"] = subject;
        clone["defraimp_duedate"] = dueDate;
        clone["defraimp_originalquery"] = new QueryLookup(queryId.replace("{", "").replace("}", ""), subject!, "defraimp_importquery");

        console.log("Defined clone");

        let formOptions: any = {};
        formOptions["entityName"] = "defraimp_importquery";

        console.log("Defined form options");

        Xrm.Navigation.openForm(formOptions, clone);
    }

    class QueryLookup {

        id: string;
        name: string;
        entityType: string;

        constructor(_id: string, _name: string, _entityType: string) {

            this.id = _id;
            this.name = _name;
            this.entityType = _entityType;
        }
    }
}