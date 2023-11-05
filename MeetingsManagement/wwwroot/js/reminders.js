const remindersContainer = document.querySelector(".content .container .holder .reminders");
const newReminderForm = document.querySelector(".content .container .holder .new-reminder-form");
const typeSelectInput = document.querySelector(".content .container .holder .new-reminder-form select[name=\"typeId\"]");
// [0]: reminder, [1]: element
const reminderPairs = [];

reminderTypesMapper = new Map(reminderTypes.map(type => [type.Id, type.Type]));

const createKeyValue = (name, value, className) => {
    const element = document.createElement("div");
    element.classList.add(className);
    const nameSpan = document.createElement("span");
    nameSpan.textContent = name;
    element.appendChild(nameSpan);
    const valueSpan = document.createElement("span");
    valueSpan.textContent = value;
    element.appendChild(valueSpan);
    return element;
};

const reminderRemover = (reminderId) => {
    fetch(`/Reminders/Delete/${reminderId}`, {
        method: "delete"
    }).then(response => {
        if (!response.ok)
            return;
        for (let i = 0; i < reminderPairs.length; ++i) {
            if (reminderPairs[i][0].Id == reminderId) {
                reminderPairs[i][1].remove();
                reminderPairs.splice(i, 1);
                break;
            }
        }
    });
};

const createReminderElement = (reminder) => {
    const reminderElement = document.createElement("div");
    reminderElement.classList.add("reminder");

    const deleteButton = document.createElement("div");
    deleteButton.classList.add("delete-button");
    deleteButton.textContent = "X";
    deleteButton.addEventListener("click", () => reminderRemover(reminder.Id));
    reminderElement.appendChild(deleteButton);

    reminderElement.appendChild(createKeyValue("Date", reminder.Date, "date"));
    reminderElement.appendChild(document.createElement("hr"));
    reminderElement.appendChild(createKeyValue("Time", reminder.Time, "time"));
    reminderElement.appendChild(document.createElement("hr"));
    reminderElement.appendChild(createKeyValue("Type", reminderTypesMapper.get(reminder.TypeId), "type"));

    return reminderElement;
};

reminders.forEach((reminder) => {
    const element = createReminderElement(reminder);
    remindersContainer.appendChild(element);
    reminderPairs.push([reminder, element]);
});

reminderTypes.forEach(reminderType => {
    const option = document.createElement("option");
    option.setAttribute("value", reminderType.Id);
    option.textContent = reminderType.Type;
    typeSelectInput.appendChild(option);
});

const addReminderToContainer = (reminder) => {
    if (reminderPairs.length == 0) {
        const reminderElement = createReminderElement(reminder);
        remindersContainer.appendChild(reminderElement);
        reminderPairs.push([reminder, reminderElement]);
        return;
    }
    let i = 0;
    for (; i < reminderPairs.length &&
        reminderPairs[i][0].Date <= reminder.Date &&
        reminderPairs[i][0].Time <= reminder.Time; ++i);
    const reminderElement = createReminderElement(reminder);
    if (i < reminderPairs.length) {
        remindersContainer.insertBefore(reminderElement, reminderPairs[i][1]);
    } else {
        remindersContainer.appendChild(reminderElement);
    }
    reminderPairs.splice(i, 0, [reminder, reminderElement]);
};

newReminderForm.addEventListener("submit", e => {
    e.preventDefault();
    const reminder = {};
    for (const [key, value] of new FormData(newReminderForm))
        reminder[key] = value;
    fetch("/Reminders/Create", {
        method: "post",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(reminder)
    }).then(async (response) => {
        if (!response.ok)
            return;
        addReminderToContainer(await response.json());
        newReminderForm.reset();
    });
});