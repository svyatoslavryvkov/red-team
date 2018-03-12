import { surveyConstants } from './actions';
import { surveyService } from '../services/surveyServer.js';
import { Survey } from '../../models/survey';

let nextSurveyId = 0;

function IdInc() {
    nextSurveyId += 1;
    return nextSurveyId;
}

export const addSurvey = text => ({
    type: surveyConstants.ADD_SURVEY,
    survey: new Survey(IdInc(), text),
});

export const getSurveys = () => ({
    type: surveyConstants.GET_SURVEYS,
    surveys: surveyService.getSurveys(),
});
