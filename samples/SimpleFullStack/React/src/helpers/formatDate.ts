export const formatDate = (date: string) => {
  const time = new Date(date).toLocaleTimeString('en-US');
  const dateStr = new Date(date).toLocaleDateString('en-US');

  return `${dateStr} ${time}`;
};
