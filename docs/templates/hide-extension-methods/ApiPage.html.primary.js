// Removes extension methods from API pages produced by the modern DocFX template.

exports.transform = function (model) {
  clearExtensionMethods(model);
  return model;
};

function clearExtensionMethods(node) {
  if (!node || typeof node !== "object") {
    return;
  }

  if (Array.isArray(node.extensionMethods)) {
    node.extensionMethods = [];
  }

  if (Array.isArray(node.items)) {
    for (var i = 0; i < node.items.length; i++) {
      clearExtensionMethods(node.items[i]);
    }
  }
}
