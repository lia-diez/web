'use strict';
async function deleteRecords() {
    await fetch('/api/delete');
    location.reload();
}
