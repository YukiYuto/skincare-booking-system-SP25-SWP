export const formatName = (name) => {
    console.log("Name: ", name);
  return name.split(" ").slice(0, 1).join("+");
};

export const PLACEHOLDER_IMAGE_URL = "https://eu.ui-avatars.com/api/?name=";
