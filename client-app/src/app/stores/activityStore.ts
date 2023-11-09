import { makeAutoObservable } from "mobx"
import { Activity } from "../models/activity"
import agent from "../api/agent";

export default class ActivityStore {
    activities: Activity[] = [];
    selectedActivity: Activity | null = null;
    edsitMode = false;
    loadiing = false;
    loadiingInitial = false;

    constructor() {
        makeAutoObservable(this)
    }

    loadActivities = async () => {
        this.loadiingInitial = true;

        try {
            const activities = await agent.Activities.list();
            activities.forEach(activity => {
                activity.date = activity.date.split('T')[0]
                this.activities.push(activity);
              })

              this.loadiingInitial = false;
        } catch(error) {
            console.log(error);
            this.loadiingInitial = false;
        }
    }
}