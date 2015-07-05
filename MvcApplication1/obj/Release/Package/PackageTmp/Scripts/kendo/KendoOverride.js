kendo.data.Model.fn.set = function (field, value, initiator) {
    var that = this;

    if (that.editable(field)) {
        value = that._parse(field, value);

        if (value != that.get(field)) {
            that.dirty = true;


            var data = this.parent();
            for (var i = 0; i < data.length; i++) {
                if (data[i].id === that.id) {
                    data[i].dirty = true;
                    for (var prop in data[i]) {
                        if (prop == field) {
                            data[i][prop] = value;
                        }
                    }

                }

            }


           // ObservableObject.fn.set.call(that, field, value, initiator);
        }
    }
}