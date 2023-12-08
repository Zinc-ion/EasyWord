async function setImage(imageStream, id) {
    const arrayBuffer = await imageStream.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const image = document.getElementById(id);
    image.onload = () => URL.revokeObjectURL(url);
    image.src = url;
}