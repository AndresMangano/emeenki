export function getHeaders() {
    return {
        headers: {
            Authorization: `Bearer ${localStorage.getItem('hermes.token' || '')}`
        }
    };
}