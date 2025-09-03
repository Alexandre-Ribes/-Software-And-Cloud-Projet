import axios from 'axios';

export const sendDirectionToSnakeState = async (id: string, directions: string[]) => {
  try {
    await axios.post(`http://localhost:8003/snake/${id}/on-update`, {
      directions
    });
    console.log(`Sent directions ${directions.join(",")} for player ${id}`);
  } catch (err) {
    console.error(`Error sending directions for ${id}`, err);
  }
};