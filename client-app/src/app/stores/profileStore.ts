import { makeAutoObservable, runInAction } from "mobx";
import { Profile } from "../models/profile";
import agent from "../api/agent";
import { store } from "./store";

export default class ProfileStore {
    profile: Profile | null = null;
    loadingProfile = false;

    constructor() {
        makeAutoObservable(this);
    }

    get isCurrentUser() {
        console.log(store.userStore.user)
        console.log(this.profile)
        if(store.userStore.user && this.profile) {
            console.log(store.userStore.user.userName)
            console.log(this.profile.username)
            return store.userStore.user.userName === this.profile.username;
        }

        return false;
    }

    loadProfile = async (username: string) => {
        this.loadingProfile = true;
        try {
            const profile = await agent.Profiles.get(username);
            runInAction(() => {
                this.profile = profile;
                this.loadingProfile = false;
            })
        } catch(error) {
            console.log(error);
            runInAction(() => this.loadingProfile = false);
        }
    }
}