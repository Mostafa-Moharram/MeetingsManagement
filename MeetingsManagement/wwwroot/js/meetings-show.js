const meetingCards = document.querySelectorAll(".content .container .meetings .meeting");

meetingCards.forEach((meetingCard) => {
    meetingCard.querySelector(".delete-button").addEventListener("click", () => {
        const index = meetingCard.getElementsByTagName("input")[0].value;
        sendDeleteMeeting(index, () => meetingCard.remove());
    });
});

const sendDeleteMeeting = (index, onSuccess, onFailure) => {
    fetch("/Meetings/Delete", {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        },
        body: index
    }).then(response => response.json()).then(response => {
        if (response.status === "Succeeded") {
            if (onSuccess !== undefined && onSuccess !== null)
                onSuccess();
            return;
        }
        if (response.status === "Failed") {
            if (onFailure !== undefined && onFailure !== null)
                onFailure();
            return;
        }
    });
};