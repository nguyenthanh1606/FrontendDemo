Vue.directive('select2', {
    twoWay: true,
    bind: function (el, binding, vnode) {
        $(el).select2({
            theme: "bootstrap",
            language: binding.value.lang
        }).on("select2:select", function (e) {
            this.set($(this.el).val());
            binding.value.model = $(this.el).val();
            alert(binding.value.model);
        }.bind(this));
    }
});

var productGroupMixin = {
    data: {
        propertyPerPage: 10,
        listGroupParentProperty: null,
        isAddingNewGroupProperty: false,
        newGroupProperty: null,
        isEditUrl: false,
        currentUrl: '',
        listDeletedGroupProperties: [],
        listDeletedProductProperties: []
    },
    components: {
        groupPropertyItem: {
            props: ['group', 'index', 'propertyType', 'propertyPerPage', 'propertyPrototype', 'isParentProp'],
            template: '#groupProperty-template',
            data: function () {
                return {
                    currentPage: 1,
                    isAddingNewProperty: false,
                    newProperty: null,
                    filterTitle: '',
                    filterNote: '',
                    currentProperty: null,
                    isEditing: false,
                    originalValue: null,
                }
            },
            created: function () {
                this.newProperty = JSON.parse(JSON.stringify(this.propertyPrototype));
                var groupProperty = this.group;
                this.originalValue = { Title: groupProperty.Title, Type: groupProperty.Type, Priority: groupProperty.Priority, AllowFilter: groupProperty.AllowFilter };
            },
            components: {
                propertyItem: {
                    template: '#property-template',
                    props: ['property', 'group', 'index', 'parentIndex'],
                    data: function () {
                        return {
                            tempValue: null,
                            isEditing: false,
                            currentProperty: null,
                        }
                    },
                    created: function () {
                        this.tempValue = JSON.parse(JSON.stringify(this.property));
                    },
                    methods: {
                        SaveProperty: function () {
                            this.property.Title = this.tempValue.Title;
                            this.property.Note = this.tempValue.Note;
                            this.property.Priority = this.tempValue.Priority;
                            this.property.PictureUrl = this.tempValue.PictureUrl;
                            this.isEditing = false;
                            this.property.isDirty = true;
                        },
                        OpenVueElf: function (property) {
                            var self = this;
                            currentProperty = self;
                            if (ElfInput.elfNode == undefined) {
                                ElfInput.elfNode = $('<div \>');
                                ElfInput.elfNode.dialog({
                                    modal: true,
                                    width: '80%',
                                    title: 'Server File Manager',
                                    create: function (event, ui) {
                                        var startPathHash = (ElfInput.elfDirHashMap[ElfInput.dialogName] && ElfInput.elfDirHashMap[ElfInput.dialogName]) ? ElfInput.elfDirHashMap[ElfInput.dialogName] : '';
                                        // elFinder configure
                                        ElfInput.elfInsrance = $(this).elfinder({
                                            startPathHash: startPathHash,
                                            useBrowserHistory: false,
                                            resizable: false,
                                            width: '100%',
                                            url: ElfInput.elfUrl,
                                            lang: ElfInput.elfLang,
                                            getFileCallback: function (file) {
                                                currentProperty.ChangePictureUrl(file);
                                                ElfInput.elfNode.dialog('close');
                                                ElfInput.elfInsrance.disable();

                                            }
                                        }).elfinder('instance');
                                    },
                                    open: function () {
                                        ElfInput.elfNode.find('div.elfinder-toolbar input').blur();
                                        setTimeout(function () {
                                            ElfInput.elfInsrance.enable();
                                        }, 100);
                                    },
                                    resizeStop: function () {
                                        ElfInput.elfNode.trigger('resize');
                                    }
                                }).parent().css({ 'zIndex': '11000' });
                            }
                            else {
                                if (ElfInput.elfDirHashMap[ElfInput.dialogName] && ElfInput.elfDirHashMap[ElfInput.dialogName] != ElfInput.elfInsrance.cwd().hash) {
                                    ElfInput.elfInsrance.request({
                                        data: { cmd: 'open', target: ElfInput.elfDirHashMap[dialogName] },
                                        notify: { type: 'open', cnt: 1, hideCnt: true },
                                        syncOnFail: true
                                    });
                                }
                                ElfInput.elfNode.dialog('open');
                            }
                        },
                        ChangePictureUrl: function (file) {
                            this.tempValue["PictureUrl"] = file;
                        },
                    },
                },
            },
            computed: {
                filteredData: function () {
                    var self = this;
                    var groupProperty = self.group;
                    var list = groupProperty.ListProductProperty;
                    if (self.filterTitle) {
                        var filterTitle = self.filterTitle.toLowerCase();
                        list = list.filter(function (row) {
                            return row.Title.toLowerCase().indexOf(filterTitle) !== -1;
                        });
                    }
                    if (self.filterNote) {
                        var filterNote = self.filterNote.toLowerCase();
                        list = list.filter(function (row) {
                            return row.Note.toLowerCase().indexOf(filterNote) !== -1;
                        });
                    }
                    return list;
                },
                pagedData: function () {
                    if (this.filteredData) {
                        return this.filteredData.slice((this.currentPage - 1) * this.propertyPerPage, this.currentPage * this.propertyPerPage);
                    }
                    else {
                        return [];
                    }
                },
                totalPages: function () {
                    var self = this;
                    if (self.filteredData) {
                        return Math.ceil(self.filteredData.length / self.propertyPerPage);
                    }
                    else {
                        return 1;
                    }
                },
                parentTypeName: function () {
                    var self = this;
                    var result = self.propertyType.filter(function (v) {
                        return v.Id === self.group.Type;
                    })[0].Name;
                    return result;
                }
            },
            methods: {
                DeletePropertyRow: function (index) {
                    var self = this;
                    var groupProperty = self.group;
                    var deletedProp = groupProperty.ListProductProperty[index];
                    if (deletedProp.ProductPropertyID) {
                        self.$parent.listDeletedProductProperties.push(deletedProp);
                    }
                    groupProperty.ListProductProperty.splice(index, 1)
                },
                AddNewProperty: function () {
                    var self = this;
                    var groupProperty = self.group;

                    if (self.newProperty.Title) {
                        groupProperty.ListProductProperty.push(JSON.parse(JSON.stringify(self.newProperty)));
                        self.newProperty.Note = '';
                        self.newProperty.Title = '';
                        self.isAddingNewProperty = false;
                    }
                },
                CancelGroupPropertyEdit: function () {
                    var self = this;
                    self.group.Title = self.originalValue.Title;
                    self.group.Type = self.originalValue.Type;
                    self.group.Priority = self.originalValue.Priority;
                    self.group.AllowFilter = self.originalValue.AllowFilter;
                    self.isEditing = false;
                },
                SaveGroupProperty: function () {
                    var self = this;
                    var groupProperty = self.group;
                    if (groupProperty.Title) {
                        self.originalValue = { Title: groupProperty.Title, Type: groupProperty.Type, Priority: groupProperty.Priority, AllowFilter: groupProperty.AllowFilter };
                        self.isEditing = false;
                        self.group.isDirty = true;
                    }
                },
            }
        },
    },
    methods: {
        DeleteRow: function (index) {
            var self = this;
            var deletedGroup = self.listGroupProperty[index];
            deletedGroup.ListProductProperty = null;
            if (deletedGroup.GroupPropertyID) {
                self.listDeletedGroupProperties.push(deletedGroup);
            }

            self.listGroupProperty.splice(index, 1)
        },
        AddNewGroupProperty: function () {
            var self = this;
            if (self.newGroupProperty.Title) {
                self.listGroupProperty.push(self.newGroupProperty);
                self.newGroupProperty = JSON.parse(JSON.stringify(self.groupPropertyPrototype));
                self.isAddingNewGroupProperty = false;
            }

        },
        GetParentGroupProperties: function () {
            var self = this;
            $.ajax({
                url: self.getParentPropertiesUrl + "/" + self.parentGroupId,
                type: 'GET',
                success: function (data) {
                    self.listGroupParentProperty = data;
                },
            });
        },
        saveUrl: function () {
            if (this.isEditUrl) {
                if ($('#FriendlyUrl').valid()) {
                    this.currentUrl = this.friendlyUrl;
                    this.originalFriendlyUrl = this.friendlyUrl;
                    this.isEditUrl = false;
                }
            }
        },
        cancelEdit: function () {
            this.friendlyUrl = this.originalFriendlyUrl;
            this.isEditUrl = false;
        },
        GetListDirty: function () {
            this.listGroupProperty.forEach(function (group) {
                group.ListProductProperty = group.ListProductProperty.filter(function (row) {
                    return row.isDirty === true;
                });
            });

            var listDirty = this.listGroupProperty.filter(function (group) {
                return group.isDirty === true || group.ListProductProperty.length > 0;
            });
            return listDirty;
        }
    },
    created: function () {
        this.groupPropertyPrototype.isDirty = true;
        this.propertyPrototype.isDirty = true;
        this.newGroupProperty = JSON.parse(JSON.stringify(this.groupPropertyPrototype));
        if (this.parentGroupId) {
            this.GetParentGroupProperties();
        }

        this.currentUrl = this.originalFriendlyUrl;
    },
    watch: {
        parentGroupId: function () {
            this.GetParentGroupProperties();
        }
    }
};

var groupMixin = {
    data: {
        isEditUrl: false,
        currentUrl: '',
    },
    methods: {
        saveUrl: function () {
            if (this.isEditUrl) {
                if (this.isUseExternalLink) {
                    if ($('#ExternalLink').valid()) {
                        this.currentUrl = this.externalLink;
                        this.isEditUrl = false;
                        this.originalExternalLink = this.externalLink;
                        this.originalIsUseExternalLink = this.isUseExternalLink;
                    }
                }
                else {
                    if ($('#FriendlyUrl').valid()) {
                        this.currentUrl = this.friendlyUrl;
                        this.isEditUrl = false;
                        this.originalFriendlyUrl = this.friendlyUrl;
                        this.originalIsUseExternalLink = this.isUseExternalLink;
                    }
                }
            }
        },
        cancelEdit: function () {
            if (this.isUseExternalLink) {
                this.externalLink = this.originalExternalLink;
            }
            else {
                this.friendlyUrl = this.originalFriendlyUrl;
            }
            this.isEditUrl = false;
            this.isUseExternalLink = this.originalIsUseExternalLink;
        }
    },
    created: function () {
        if (this.isUseExternalLink) {
            this.currentUrl = this.originalExternalLink;
        }
        else {
            this.currentUrl = this.originalFriendlyUrl;
        }
    },
    watch: {
        isUseExternalLink: function (value) {
            if (value) {
                this.friendlyUrl = this.originalFriendlyUrl;
            }
            else {
                this.externalLink = this.originalExternalLink;
            }
        }
    }
}

var contentMixin = {
    data: {
        isEditUrl: false,
        currentUrl: '',
    },
    methods: {
        saveUrl: function () {
            if (this.isEditUrl) {
                if ($('#FriendlyUrl').valid()) {
                    this.currentUrl = this.friendlyUrl;
                    this.originalFriendlyUrl = this.friendlyUrl;
                    this.isEditUrl = false;
                }
            }
        },
        cancelEdit: function () {
            this.friendlyUrl = this.originalFriendlyUrl;
            this.isEditUrl = false;
        }
    },
    created: function () {
        this.currentUrl = this.originalFriendlyUrl;
    }
}