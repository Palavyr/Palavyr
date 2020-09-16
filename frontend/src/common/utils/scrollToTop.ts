let globLastC = Infinity;

const scrollToTopSmoothlyRaw = () => {
  const h = document.documentElement.scrollTop || document.body.scrollTop;
  if (h > 0 && globLastC > h) {
    globLastC = h;
    window.requestAnimationFrame(scrollToTopSmoothlyRaw);
    window.scrollTo(0, h - h / 8);
  } else {
    globLastC = Infinity;
  }
}

export const scrollToTop = () => {
  setTimeout(() => {
    scrollToTopSmoothlyRaw();
  }, 10);
}

export default scrollToTop;
