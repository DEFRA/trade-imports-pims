var DefraImports;
(function (DefraImports) {
    var ImportQuery;
    (function (ImportQuery) {
        function CloneImportQueryButton(primaryControl) {
            console.log("Function called!");
            var emailToSend = primaryControl.getAttribute("defraimp_querysentto").getValue();
            console.log(emailToSend);
            var subject = primaryControl.getAttribute("subject").getValue();
            console.log(subject);
            var dueDate = primaryControl.getAttribute("defraimp_duedate").getValue();
            console.log(dueDate.toString());
            var queryId = primaryControl.data.entity.getId();
            console.log(queryId);
            var clone = {};
            clone["defraimp_querysentto"] = emailToSend;
            clone["subject"] = subject;
            clone["defraimp_duedate"] = dueDate;
            clone["defraimp_originalquery"] = new QueryLookup(queryId.replace("{", "").replace("}", ""), subject, "defraimp_importquery");
            console.log("Defined clone");
            var formOptions = {};
            formOptions["entityName"] = "defraimp_importquery";
            console.log("Defined form options");
            Xrm.Navigation.openForm(formOptions, clone);
        }
        ImportQuery.CloneImportQueryButton = CloneImportQueryButton;
        var QueryLookup = /** @class */ (function () {
            function QueryLookup(_id, _name, _entityType) {
                this.id = _id;
                this.name = _name;
                this.entityType = _entityType;
            }
            return QueryLookup;
        }());
    })(ImportQuery = DefraImports.ImportQuery || (DefraImports.ImportQuery = {}));
})(DefraImports || (DefraImports = {}));
//# sourceMappingURL=importquery.ribbon.js.map